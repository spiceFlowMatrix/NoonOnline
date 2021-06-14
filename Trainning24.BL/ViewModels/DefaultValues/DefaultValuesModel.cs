using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.DefaultValues
{
    public class DefaultValuesModel
    {
        public long id { get; set; }
        public int timeout { get; set; }
        public int reminder { get; set; }
        public bool istimeouton { get; set; }
        public int intervals { get; set; }
    }
}
