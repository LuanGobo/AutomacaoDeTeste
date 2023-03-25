using PLC.Program;
using Tags;

namespace PLC
{
    internal class Global
    {
        public List<BlockProgram> sheets_list { get; set; }
        public List<Tag> global_tags_list { get; set; }

        public Global() { }

    }
}
