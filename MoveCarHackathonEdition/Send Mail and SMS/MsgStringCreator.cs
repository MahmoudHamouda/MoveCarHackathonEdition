using MoveCarHackathonEdition.Data_Access_Layer;

namespace MoveCarHackathonEdition.Send_Mail_and_SMS
{
    public static class MsgStringCreator
    {
        public static string MsgToCarBlocked(CAR_USER_VIEW caruserBlocker)
        {
            if (caruserBlocker.PERSONAL_MOBILE == caruserBlocker.WORK_MOBILE)
            {
                return
                    $"Your car blocker details: {caruserBlocker.EMPLOYEE_NAME} \n " +
                    $"{caruserBlocker.EMAIL} \n Dept: {caruserBlocker.DEPARTMENT} \n Work Number: {caruserBlocker.EXTENSION} \n Manager: {caruserBlocker.MANAGER_NAME}";
            }
            return
                $"Your car blocker details: {caruserBlocker.EMPLOYEE_NAME} \n {caruserBlocker.EMAIL} \n " +
                $"Dept: {caruserBlocker.DEPARTMENT} \n Work Number: {caruserBlocker.EXTENSION} \n Work Mobile:{caruserBlocker.WORK_MOBILE} \n Manager: {caruserBlocker.MANAGER_NAME}";
        }

        public static string MsgToCarBlocked()
        {
            return
                "Sorry, we could not find a match for this car plate number in the database. So, we are now sending an email to All EMC Egypt Employees.";
        }

        public static string MsgToCarBlocker(CAR_USER_VIEW caruserBlocked)
        {
            if (caruserBlocked.PERSONAL_MOBILE == caruserBlocked.WORK_MOBILE)
            {
                return
                    $"Please move your car, you are blocking : {caruserBlocked.EMPLOYEE_NAME} \n " +
                    $"{caruserBlocked.EMAIL} \n Dept: {caruserBlocked.DEPARTMENT} \n Work Number: {caruserBlocked.EXTENSION}";
            }
            return
                $"Please move your car, you are blocking : {caruserBlocked.EMPLOYEE_NAME} \n " +
                $"{caruserBlocked.EMAIL} \n Dept: {caruserBlocked.DEPARTMENT} \n Work Number: {caruserBlocked.EXTENSION} \n Work Mobile:{caruserBlocked.WORK_MOBILE}";
        }

        public static string MsgToCarBlocker()
        {
            return
                "Kindly move your car as it blocks others to leave the office.";
        }
    }
}