using System;
using MoveCarHackathonEdition.Data_Access_Layer;
using MoveCarHackathonEdition.MySqlNS;

namespace MoveCarHackathonEdition.Events
{
    public class EmailSmsEventArgs : EventArgs
    {
        public CAR_USER_VIEW CarUserBlocker { get; set; }
        public CAR_USER_VIEW CarUserBlocked { get; set; }
        public car_request Req { get; set; }
    }
    public class EmailSmsNoBlockerEventArgs : EventArgs
    {
        public CAR_USER_VIEW CarUserBlocked { get; set; }
        public car_request Req { get; set; }
    }
    public class EmailSmsNoBlockedEventArgs : EventArgs
    {
        public CAR_USER_VIEW CarUserBlocker { get; set; }
        public car_request Req { get; set; }
    }
}