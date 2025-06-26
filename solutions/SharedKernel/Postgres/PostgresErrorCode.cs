namespace SharedKernel;

public enum PostgresErrorCode: int
{
    UniqueViolation = 23505,
    ForeignKeyViolation = 23503,
    CheckViolation = 23514,
    NotNullViolation = 23502,
    SerializationFailure = 40001,
    TriggeredDataChangeViolation = 25006,
    DataTooLong = 2200,
    NumericValueOutOfRange = 22003,
    StringDataRightTruncation = 22001,
    InvalidTextRepresentation = 22007,
    InvalidBinaryRepresentation = 22008,
    InvalidEscapeCharacter = 22009,
    InvalidDatetimeFormat = 22007
}

