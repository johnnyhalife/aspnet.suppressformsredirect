// <copyright file="SuppressFormsAuthenticationRedirectModule.cs" company="open-source" >
//  No rights reserved. Copyright (c) 2011 by Johnny Halife, Juan Pablo Garcia, Mauro Krikorian, Mariano Converti, 
//                                            Damian Martinez, Nico Bello, and Ezequiel Morito
//   
//  Redistribution and use in source and binary forms, with or without modification, are permitted.
//
//  The names of its contributors may not be used to endorse or promote products derived from this software without specific prior written permission.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
namespace System.Web
{
    using System;
    using System.Linq;
    using System.Net;

    /// <summary>
    /// HACK: Module that prevents ASP.NET Forms Authentication to redirect the 
    /// user to the login page. This is helpful for AJAX, JSON, and all other 
    /// of non Web Representation (Views/Pages) type of requests. As we don't want to 
    /// always suppress it you should mark either the response (by adding a header) or
    /// the HttpContext.Items with the proposed flag.
    /// <para />
    /// It will completetly interrupt the ongoing request by the request end,
    /// no redirect signal will be send back to the requestor.
    /// </summary>
    public class SuppressFormsAuthenticationRedirectModule : IHttpModule
    {
        /// <summary>
        /// Context Item Key used for suppression when it's HttpContext.Items-based.
        /// </summary>
        public const string SuppressFormsAuthenticationKey = "formsRedirect.suppress";

        /// <summary>
        /// Custom Header used for suppression when it's response-based.
        /// </summary>
        public const string SuppressFormsHeaderName = "X-FormsRedirect-Suppress";

        /// <summary>
        /// Initalizes a new instance of the module.
        /// </summary>
        /// <param name="context">Ongoing HttpContext.</param>
        public void Init(HttpApplication context)
        {
            context.EndRequest += this.RequestEnded;
        }

        /// <summary>
        /// Uselss. Required by the IHttpModule interface.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Handles the RequestEnd event, this is where we are inspecting
        /// the parameters and the mark to stop the ongoing response from 
        /// getting redirected if needed.
        /// </summary>
        /// <param name="source">Current application.</param>
        /// <param name="args">Useless event args.</param>
        private void RequestEnded(object source, EventArgs args)
        {
            var context = (HttpApplication)source;

            if (!this.ShouldSuppress(context))
                return;

            context.Response.TrySkipIisCustomErrors = true;
            context.Response.ClearContent();
            context.Response.StatusCode = 401;
            context.Response.RedirectLocation = null;
            context.Response.Flush();
            context.Response.End();
        }

        /// <summary>
        /// Returns a value indicating whether the current redirect
        /// should be suppress based on the header and/or http context items.
        /// </summary>
        /// <param name="context">Ongoing application context.</param>
        /// <returns>A value indicating whether the current redirect should be prevented.</returns>
        private bool ShouldSuppress(HttpApplication context)
        {
            return context.Context.Items.Contains(SuppressFormsAuthenticationKey)
                || (HttpRuntime.UsingIntegratedPipeline && context.Response.Headers.AllKeys.Contains(SuppressFormsHeaderName));
        }
    }
}