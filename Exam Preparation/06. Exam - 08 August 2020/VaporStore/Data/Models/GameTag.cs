namespace VaporStore.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class GameTag
    {
        public int GameId { get; set; }

        [ForeignKey(nameof(GameId))]
        public virtual Game Game { get; set; } = null!;

        public int TagId { get; set; }

        [ForeignKey(nameof(TagId))]
        public virtual Tag Tag { get; set; } = null!;
    }
}

