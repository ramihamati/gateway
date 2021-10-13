using Digitteck.Gateway.Service;
using Digitteck.Gateway.Service.Common.DataValidator;
using Digitteck.Gateway.Service.ComponentConfigurationValidator;
using NUnit.Framework;
using System.Collections.Generic;
#pragma warning disable IDE1006 // Naming Styles

namespace ValidatorTests
{
    public sealed class ValidationOperationReturnGeneralTests
    {
        [Test]

        public void validate_when_no_operations_are_defined()
        {
            GatewayConfiguration config = Fixture.GetConfig();

            config.RouteModels = new List<RouteDefinition> {
                    new RouteDefinition
                    {
                        Downstream = new Downstream
                        {
                            Operations = new List<OperationCore>()
                        }
                    }
            };

            ValidationOperationReturnGeneral validation = new ValidationOperationReturnGeneral();
            ValidationMessage message = validation.Validate(config);

            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_no_operation_is_a_return()
        {
            GatewayConfiguration config = Fixture.GetConfig();

            config.RouteModels = new List<RouteDefinition> {
                    new RouteDefinition
                    {
                        Downstream = new Downstream
                        {
                            Operations = new List<OperationCore>
                            {
                                new OperationCall()
                            }
                        }
                }
            };

            ValidationOperationReturnGeneral validation = new ValidationOperationReturnGeneral();
            ValidationMessage message = validation.Validate(config);

            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_the_return_operation_is_not_last()
        {
            GatewayConfiguration config = Fixture.GetConfig();

            config.RouteModels = new List<RouteDefinition> {
                    new RouteDefinition
                    {
                        Downstream = new Downstream
                        {
                            Operations = new List<OperationCore>
                            {
                                new OperationReturn(),
                                new OperationCall()
                            }
                    }
                }
            };

            ValidationOperationReturnGeneral validation = new ValidationOperationReturnGeneral();
            ValidationMessage message = validation.Validate(config);

            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_there_are_multiple_return_operations()
        {
            GatewayConfiguration config = Fixture.GetConfig();

            config.RouteModels = new List<RouteDefinition> {
                    new RouteDefinition
                    {
                        Downstream = new Downstream
                        {
                            Operations = new List<OperationCore>
                            {
                                new OperationReturn(),
                                new OperationCall(),
                                new OperationReturn()
                            }
                        }
                }
            };

            ValidationOperationReturnGeneral validation = new ValidationOperationReturnGeneral();
            ValidationMessage message = validation.Validate(config);

            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_the_operation_return_is_correctly_placed()
        {
            GatewayConfiguration config = Fixture.GetConfig();

            config.RouteModels = new List<RouteDefinition> {
                    new RouteDefinition
                    {
                        Downstream = new Downstream
                        {
                            Operations = new List<OperationCore>
                            {
                                new OperationCall(),
                                new OperationReturn()
                            }
                        }
                }
            };

            ValidationOperationReturnGeneral validation = new ValidationOperationReturnGeneral();
            ValidationMessage message = validation.Validate(config);

            Assert.IsTrue(message.IsValid);
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles