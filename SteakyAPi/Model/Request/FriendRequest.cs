namespace StreakyAPi.Model.Request
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public int RequesterId { get; set; }
        public int ReceiverId { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime RequestDate { get; set; }

        public UserAccount Requester { get; set; }
        public UserAccount Receiver { get; set; }
    }
}