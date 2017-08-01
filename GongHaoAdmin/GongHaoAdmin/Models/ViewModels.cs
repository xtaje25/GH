using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GongHaoAdmin.Models
{
    public class ViewModels { }

    public class VM_Page<T>
    {
        public int pageNum { get; set; }
        public int numPerPage { get; set; }
        public int totalcount { get; set; }
        public int[] option { get; set; }
        public List<T> list { get; set; }
    }
}