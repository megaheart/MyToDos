using Storage.Model;
using System.Collections.Generic;
using Xunit;

namespace MyToDos.Test
{
    public class CollectionForIdentificationObjectTest
    {
        [Theory]
        [InlineData(-1, "c7w")]
        [InlineData(0, "7nc7w")]
        [InlineData(1, "AADPV")]
        [InlineData(-1, "AADPp")]
        [InlineData(4, "MNTqj")]
        [InlineData(6, "1ce7BB")]
        [InlineData(7, "1eCPIu")]
        [InlineData(8, "1fh9qF")]
        [InlineData(-1, "1fhIqF")]
        [InlineData(11, "1sjKPj")]
        [InlineData(12, "1sTq2L")]
        [InlineData(13, "23ERaO")]
        [InlineData(14, "27vz0p")]
        [InlineData(-1, "c7wdfsa")]
        public void IndexOfID_Test1(int index, string id)
        {
            List<Tag> list = new List<Tag>()
            {
                new Tag(false,"0","7nc7w",""),
                new Tag(false,"1","AADPV",""),
                new Tag(false,"2","jjTJp",""),
                new Tag(false,"3","klLkf",""),
                new Tag(false,"4","MNTqj",""),
                new Tag(false,"5","yPe5r",""),
                new Tag(false,"6","1ce7BB",""),
                new Tag(false,"7","1eCPIu",""),
                new Tag(false,"8","1fh9qF",""),
                new Tag(false,"9","1gfkvT",""),
                new Tag(false,"10","1MIgBX",""),
                new Tag(false,"11","1sjKPj",""),
                new Tag(false,"12","1sTq2L",""),
                new Tag(false,"13","23ERaO",""),
                new Tag(false,"14","27vz0p","")
            };
            CollectionForIdentificationObject<Tag> tasks = new CollectionForIdentificationObject<Tag>(list);
            Assert.Equal(tasks.IndexOfID(id), index);
            //Assert.Equal(list.FindIndex(i => i.ID == id), index);
        }
        [Theory]
        [InlineData(-1, "c7w")]
        [InlineData(0, "7nc7w")]
        [InlineData(1, "AADPV")]
        [InlineData(-1, "AADPp")]
        [InlineData(4, "MNTqj")]
        [InlineData(5, "yPe5r")]
        [InlineData(6, "1ce7BB")]
        [InlineData(7, "1eCPIu")]
        [InlineData(8, "1fh9qF")]
        [InlineData(-1, "1fhIqF")]
        [InlineData(11, "1sjKPj")]
        [InlineData(12, "1sTq2L")]
        [InlineData(13, "23ERaO")]
        [InlineData(-1, "c7wdfsa")]
        public void IndexOfID_Test2(int index, string id)
        {
            List<Tag> list = new List<Tag>()
            {
                new Tag(false,"0","7nc7w",""),
                new Tag(false,"1","AADPV",""),
                new Tag(false,"2","jjTJp",""),
                new Tag(false,"3","klLkf",""),
                new Tag(false,"4","MNTqj",""),
                new Tag(false,"5","yPe5r",""),
                new Tag(false,"6","1ce7BB",""),
                new Tag(false,"7","1eCPIu",""),
                new Tag(false,"8","1fh9qF",""),
                new Tag(false,"9","1gfkvT",""),
                new Tag(false,"10","1MIgBX",""),
                new Tag(false,"11","1sjKPj",""),
                new Tag(false,"12","1sTq2L",""),
                new Tag(false,"13","23ERaO","")
            };
            CollectionForIdentificationObject<Tag> tasks = new CollectionForIdentificationObject<Tag>(list);
            Assert.Equal(tasks.IndexOfID(id), index);
            //Assert.Equal(list.FindIndex(i => i.ID == id), index);
        }
    }
}
