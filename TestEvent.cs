using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EFCoreOverride
{
    public abstract class TestEvent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Time { get; set; }

        [MaxLength(50)]
        public virtual string Type { get; set; }

        public virtual string Content { get; set; }
    }

    public class FileUploadData
    {
        public string Filename { get; set; }
        public string UploadUrl { get; set; }
    }

    public class FileUploadEvent : TestEvent
    {
        public override string Type => "FileUploaded";
        public override string Content
        {
            get => JsonSerializer.Serialize(Data);
            set => Data = JsonSerializer.Deserialize<FileUploadData>(value);
        }

        [NotMapped]
        public FileUploadData Data { get; set; }
    }
}
