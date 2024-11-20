public interface UniverslaResourceHolderInterface
{

    public abstract void ClearResources();

    public virtual bool HaveFreeSpace() 
    {
        return true;
    }

    public virtual int GetFreeSpaceCounter() 
    {
        return 1;
    }

    public abstract bool HaveResource();

    public abstract int GetCountOfResources();

    //Need to be one of to be implemented (ResourceType or Resource)
    public virtual void PutResourceType(ResourceType resource)
    {
        PutResource(ResourceController.Instance.resourceDictionary[resource]);
    }

    //Need to be one of to be implemented (ResourceType or Resource)
    public virtual void PutResource(Resource resource)
    {
        PutResourceType(resource.rType);
    }

    //Need to be one of to be implemented (ResourceType or Resource)
    public virtual ResourceType PullResourceType() 
    {
        return PullResource().rType;
    }

    //Need to be one of to be implemented (ResourceType or Resource)
    public virtual Resource PullResource()
    {
        return ResourceController.Instance.resourceDictionary[PullResourceType()];
    }


}
