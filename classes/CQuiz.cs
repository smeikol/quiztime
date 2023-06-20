using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace quiztime.classes
{
    public class CQuiz
    {
        public CQuiz() { }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Picture { get; set; }

        public ObservableCollection<CQuiz> MyList { get; set; }
    }
}
