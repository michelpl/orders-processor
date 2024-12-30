using SalesProcessor.Domain.Interfaces;

namespace SalesProcessor.Domain.Exceptions;

public class InvalidFileException(string message) : Exception(message);

