#ASP.NET Suppress Forms Authentication Redirect
Module that prevents ASP.NET Forms Authentication to redirect the user to the login page. This is helpful for AJAX, 
JSON, and all other of non Web Representation (Views/Pages) type of requests.

For more information read Phill Haack's post about this http://haacked.com/archive/2011/10/04/prevent-forms-authentication-login-page-redirect-when-you-donrsquot-want.aspx

##Installation 

Pull it from the online NuGet source, as easy as...

    install-package aspnet.suppressformsredirect


##Usage

You can use it from anywhere on your App, where you want to send a 401. We provide two approaches using HttpContext Items

    HttpContext.Current.Items.Add(SuppressFormsAuthenticationRedirectModule.SuppressFormsAuthenticationKey, "true");

Or you are using WCF Web API (or something like MVC with more control on the Response) just include a custom header

    var response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
    response.Headers.Add(SuppressFormsAuthenticationRedirectModule.SuppressFormsHeaderName, "true");

    throw new HttpResponseException(response);

##License
Copyright (C) 2011 by Johnny Halife, Juan Pablo Garcia, Mauro Krikorian, Mariano Converti, 
					  Damian Martinez, Nico Bello, and Ezequiel Morito


Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.