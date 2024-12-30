using OrdersProcessor.Domain.Interfaces;

namespace OrdersProcessor.Domain.Exceptions;

public class InvalidFileException(string message) : Exception(message);

