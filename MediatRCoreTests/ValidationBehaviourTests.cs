using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Exceptions;
using LightestNight.System.Command.MediatR;
using MediatR;
using Moq;
using Shouldly;
using Xunit;

namespace LightestNight.System.Command.MediatRTests
{
    public class ValidationBehaviourTests
    {
        private const string ErrorText = "Test Error Text";
        private const string ParameterName = "Test Parameter";
        
        public class Request
        {
            public Guid Id { get; set; }
        }

        public class Response
        {
            public Guid Id { get; set; }
        }

        public class Validator : ICommandValidator<Request>
        {
            public bool Success { get; }

            public Validator(bool success)
            {
                Success = success;
            }

            public Task<IEnumerable<InvariantError>> Validate(Request command, CancellationToken cancellationToken)
            {
                return Task.FromResult(Success
                    ? Enumerable.Empty<InvariantError>()
                    : new[]
                    {
                        new InvariantError
                        {
                            ErrorText = ErrorText,
                            Parameter = ParameterName
                        }
                    });
            }
        }

        private readonly Mock<IEnumerable<ICommandValidator<Request>>> _validatorsMock = new Mock<IEnumerable<ICommandValidator<Request>>>();
        private readonly ValidationBehaviour<Request, Response> _sut;

        public ValidationBehaviourTests()
        {
            _sut = new ValidationBehaviour<Request, Response>(_validatorsMock.Object);
        }

        [Fact]
        public async Task Should_Throw_Exception_When_Validation_Fails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new Request {Id = id};
            var validators = new List<ICommandValidator<Request>>
            {
                new Validator(false)
            };
            _validatorsMock.Setup(validator => validator.GetEnumerator()).Returns(validators.GetEnumerator());
            
            // Act
            var exception = await Should.ThrowAsync<DomainException>(_sut.Handle(request, CancellationToken.None,
                Mock.Of<RequestHandlerDelegate<Response>>()));
            
            // Assert
            exception.Errors.ShouldContain(error => error.ErrorText == ErrorText && error.Parameter == ParameterName);
        }

        [Fact]
        public async Task Should_Return_Command_Response_When_No_Errors()
        {
            // Arrange
            var id = Guid.NewGuid();
            var response = new Response {Id = id};
            var validators = new List<ICommandValidator<Request>>
            {
                new Validator(true)
            };
            _validatorsMock.Setup(validator => validator.GetEnumerator()).Returns(validators.GetEnumerator());
            var requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<Response>>();
            requestHandlerDelegateMock.Setup(del => del()).Returns(Task.FromResult(response));
            
            // Act
            var result = await _sut.Handle(new Request(), CancellationToken.None,
                requestHandlerDelegateMock.Object);
            
            // Assert
            result.ShouldBe(response);
        }
    }
}