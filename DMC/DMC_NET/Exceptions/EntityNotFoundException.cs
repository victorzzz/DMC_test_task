using System;

namespace DMC_NET.Exceptions
{
    public class EntityNotFoundException : InvalidOperationException
    {
        public EntityNotFoundException(Type type, int id) 
            : base($"Entity of type {type.FullName} with id = {id} was not found")
        {

        }
    }
}
