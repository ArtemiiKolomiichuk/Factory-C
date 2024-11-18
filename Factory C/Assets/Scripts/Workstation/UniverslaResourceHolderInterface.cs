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


    public abstract void PutResourceType(ResourceType resource);

    public virtual void PutResource(Resource resource)
    {
        PutResourceType(resource.rType);
    }

    public abstract ResourceType PullResourceType();

    public virtual Resource PullResource()
    {
        return ResourceController.Instance.resourceDictionary[PullResourceType()];
    }


}
