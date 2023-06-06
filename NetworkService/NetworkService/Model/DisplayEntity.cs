using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class DisplayEntity
    {
        private Entity entity;
        private string id;

        public Entity Entity
        {
            set { entity = value; }
            get { return entity; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
