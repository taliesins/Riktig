# RIKTIG

This is a sample application that demonstrates the various way to use MassTransit, Automatonymous, Topshelf, and related projects to create applications.


## Image Request

One example has a web form where the URI of an image can be entered. The URI is then sent to a coordinator (built using Automatonymous) that
dispatches the request to the image retrieval service which ultimately retrieves the image from the server. Once retrieved, the service publishes
an event that is observed by the state machine to complete the request.

