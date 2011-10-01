﻿namespace ZeroMQ.UnitTests.ZmqSocketSpecs
{
    using System;

    using Machine.Specifications;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;

    [Subject("ZMQ Socket")]
    class when_binding_a_socket_to_a_valid_endpoint : using_mock_socket_proxy<ZmqSocket>
    {
        Establish context = () =>
            socket = new ConcreteSocket();

        Because of = () =>
            socket.Bind("tcp://*:9090");

        It should_bind_the_underlying_socket = () =>
            socketProxy.Verify(mock => mock.Bind("tcp://*:9090"));
    }

    [Subject("ZMQ Socket")]
    class when_binding_a_socket_to_a_null_endpoint : using_mock_socket_proxy<ZmqSocket>
    {
        static Exception exception;

        Establish context = () =>
            socket = new ConcreteSocket();

        Because of = () =>
            exception = Catch.Exception(() => socket.Bind(null));

        It should_fail = () =>
            exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Subject("ZMQ Socket")]
    class when_binding_a_socket_to_an_empty_endpoint : using_mock_socket_proxy<ZmqSocket>
    {
        static Exception exception;

        Establish context = () =>
            socket = new ConcreteSocket();

        Because of = () =>
            exception = Catch.Exception(() => socket.Bind(null));

        It should_fail = () =>
            exception.ShouldBeOfType<ArgumentException>();
    }

    [Subject("ZMQ Socket")]
    class when_binding_is_interrupted_by_context_termination : using_mock_socket_proxy<ZmqSocket>
    {
        static Exception exception;

        Establish context = () =>
        {
            socket = new ConcreteSocket();

            socketProxy.Setup(mock => mock.Bind(Moq.It.IsAny<string>())).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eterm);
        };

        Because of = () =>
            exception = Catch.Exception(() => socket.Bind("tcp://*:9090"));

        It should_not_fail = () =>
            exception.ShouldBeNull();
    }

    [Subject("ZMQ Socket")]
    class when_binding_and_the_proxy_returns_an_error : using_mock_socket_proxy<ZmqSocket>
    {
        static Exception exception;

        Establish context = () =>
        {
            socket = new ConcreteSocket();

            socketProxy.Setup(mock => mock.Bind(Moq.It.IsAny<string>())).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eaddrnotavail);
        };

        Because of = () =>
            exception = Catch.Exception(() => socket.Bind("tcp://*:9090"));

        It should_fail_with_socket_exception = () =>
            exception.ShouldBeOfType<ZmqSocketException>();
    }
}
