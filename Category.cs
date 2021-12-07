using System;

namespace TrabajoPractico4
{
    public class Category 
    {
        public int categoryId { get; set; }
        public String name { get; set; }

        public Category( String name)
        {            
            this.name = name;
        }

        public Category()
        {
            
        }
        public int CompareTo(Category other)
        {
            return name.CompareTo(other.name);
        }

        
    }
}