// Command Examples
// =====================================
// Feel Maria Happy 1.0
// Feel Maria Sad 0.5
// Express Maria Fear 1.0
// Express Joao Sadness 0.5
// Talk Maria Start
// Talk Maria Stop
// Talk Maria Frequency 1.0
// Talk Joao Speed 1.0
// Nod Maria Start
// Nod Maria Stop
// Nod Maria Frequency 0.5
// Nod Maria Speed 0.5
// Gazeat Maria Joao
// Gazeat Maria Speed 0.2
// Gazeat Maria Frequency 0.5
// Gazeback Maria User
// Gazeback Joao Speed 0.2
// Gazeback Joao Frequency 0.7
// MoveEyes Maria Left
// MoveEyes Maria Speed 0.2
// MoveEyes Maria Frequency 0.5

public enum ActionGroup
{
    AVATARFEEL = 1,
    EXPRESS,
    TALK,
    NOD,
    GAZEAT,
    GAZEBACK,
    MOVEEYES
}
public enum ArgumentType1
{
    MARIA = 1,
    JOAO,
    USER
}
public enum ArgumentType2
{
    START = 1,
    END
}
public enum ArgumentType3
{
    SPEED = 1,
    FREQUENCY
}