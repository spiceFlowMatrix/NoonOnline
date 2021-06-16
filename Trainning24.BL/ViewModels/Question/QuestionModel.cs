using Newtonsoft.Json;
using System.Collections.Generic;
using Trainning24.BL.ViewModels.QuestionAnswer;
using Trainning24.BL.ViewModels.QuestionFile;

namespace Trainning24.BL.ViewModels.Question
{
    public class QuestionModel
    {
        public long id { get; set; }
        public long questiontypeid { get; set; }
        public string questiontext { get; set; }
        public string explanation { get; set; }
        public bool ismultianswer { get; set; }
        public long questiontype { get; set; }
        public List<UpdateQuestionFileModel> images { get; set; }
        public List<QuestionAnswerModel> answers { get; set; }
    }

    public class QuestionModel1
    {
        public long id { get; set; }
        public long questiontypeid { get; set; }
        public string questiontext { get; set; }
        public string explanation { get; set; }
        public bool ismultianswer { get; set; }
        public long questiontype { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public QuestionStatus? Status { get; set; }
        public List<UpdateQuestionFileModel> images { get; set; }
        public List<QuestionAnswerModel1> answers { get; set; }
    }


    public enum QuestionStatus
    {
        None = 0,
        Modified = 1,
        Deleted = 2
    }
}
