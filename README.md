= ASP.NET Suppress Forms Authentication Redirect
Module that prevents ASP.NET Forms Authentication to redirect the user to the login page. This is helpful for AJAX, 
JSON, and all other of non Web Representation (Views/Pages) type of requests.

For more information read Phill Haack's post about this http://haacked.com/archive/2011/10/04/prevent-forms-authentication-login-page-redirect-when-you-donrsquot-want.aspx

== Installation 

	install-package aspnet.suppressformsredirect


== Usage

You can use it from anywhere on your App, where you want to send a 401. We provide two approaches using HttpContext Items

    HttpContext.Items.Add(SuppressFormsAuthenticationRedirectModule.SuppressFormsAuthenticationKey, "true");

Or you are using WCF Web API (or something like MVC with more control on the Response) just include a custom header

	var response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
    response.Headers.Add(SuppressFormsAuthenticationRedirectModule.SuppressFormsHeaderName, "true");

    throw new HttpResponseException(response);