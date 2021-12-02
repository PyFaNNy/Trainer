using System;

namespace Trainer.DAL.Util.Constant
{
    public enum Sex
    {
        Female = 0,
        Male = 1
    }

    public enum TypePhysicalActive
    {
        Running = 0,
        Walking = 1,
        Сycling = 2,
        Swimming = 3,
        StrengthExercises = 4
    }

    public enum Status
    {
        Active = 0,
        Finished = 1
    }

    public enum SortState
    {
        FirstNameSort,
        FirstNameSortDesc,
        LastNameSort,
        LastNameSortDesc,
        MiddleNameSort,
        MiddleNameSortDesc,
        AgeSort,
        AgeSortDesc,
        SexSort,
        SexSortDesc,
        TypeSort,
        TypeSortDesc
    }
}
