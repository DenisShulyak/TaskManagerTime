using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApplication
{
    public class Task
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; }

        public Guid ReiterationId { get; set; }
        public Reiteration Reiteration { get; set; }

        public Guid TypeOperationId { get; set; }
        public TypeOperation TypeOperation { get; set; }
    }
}
