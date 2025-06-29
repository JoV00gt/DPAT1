﻿

namespace DPAT1.Helpers
{
    public static class FSMValidationRules
    {
        public const int MAX_AUTOMATIC_TRANSITIONS_PER_STATE = 1;
        public const int MIN_TRANSITIONS_FOR_CONFLICT = 2;
        public const int SINGLE_UNREACHABLE_STATE = 1;
        public const int NO_UNREACHABLE_STATES = 0;
        public const int FIRST_ELEMENT_INDEX = 0;
        public const int NO_TRANSITIONS = 0;
        public const int MIN_GUARDS_UNIQUENESS_CHECK = 2;
    }
}
