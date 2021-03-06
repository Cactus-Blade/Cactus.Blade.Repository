using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cactus.Blade.Repository.Database
{
    public class TestProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public TestCategory Category { get; set; }

        public int CategoryId { get; set; }

        public int Stock { get; set; }

        public bool? InStock { get; set; }
    }
}