# Lightest Night
## Command > MediatR

Elements focussed on the Command side of the CQRS pattern specialised to the [MediatR library](https://github.com/jbogard/MediatR "MediatR")

### Build Status
![](https://github.com/lightest-night/system.api.rest.hypermedia/workflows/CI/badge.svg)
![](https://github.com/lightest-night/system.api.rest.hypermedia/workflows/Release/badge.svg)

#### How To Use
#### Registration
##### Validation
* Asp.Net Standard/Core Dependency Injection
  * Use the provided `services.AddCommandValidation(params Assembly[] validatorAssemblies)` method

* Other containers
  * Register all instances of `IPipelineBehavior<,>` in your container
  
##### Decorators
* Asp.Net Standard/Core Dependency Injection
  * Use the provided `serviced.AddCommandDecorators(params Assembly[] decoratorAssemblies)` method
  
* Other containers
  * Register all instances of `INotificationHandler<>` in your container
  * Register all instances of `IRequestPreProcessor<>` in your container
  * Register all instances of `IRequestPostProcessor<,>` in your container
  * Register all instances of `IRequestExceptionHandler<,,>` in your container
  * Register all instances of `IRequestExceptionAction<,>` in your container
  
#### Usage
##### Validators
* Create any Validation classes and inherit from `ICommandValidator<>`

##### Decorators
* Create any Decorator classes and inherit from `INotificationHandler<TNotification`, `IRequestPreProcessor<TRequest>`, `IRequestPostProcessor<TRequest, TResponse>`, `IRequestExceptionHandler<TRequest, TResponse, TException>`, or `IRequestExceptionAction<TRequest, TException>`

The validation and/or decorator classes will be automatically injected into the pipeline
