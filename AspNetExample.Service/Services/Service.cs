using AspNetExample.Domain.Repositories;
using AspNetExample.Service.Models;
using MyDomainModel = AspNetExample.Domain.Models.MyModel;

namespace AspNetExample.Service.Services;

public class Service : IService
{
    private readonly IRepository _repository;
    private readonly IModelDescriptionProvider _modelDescriptionProvider;

    public Service(IRepository repository, IModelDescriptionProvider modelDescriptionProvider)
    {
        _repository = repository;
        _modelDescriptionProvider = modelDescriptionProvider;
    }

    public MyModel CreateModel(string data)
    {
        var domainModel = _repository.CreateModel(data);
        return Convert(domainModel);
    }

    public MyModel GetModel(Guid id)
    {
        var domainModel = _repository.GetModel(id);
        
        if (domainModel == null) return null;

        var model = Convert(domainModel);
        string description = _modelDescriptionProvider.GetDescription(id);

        return model with { Description = description };
    }

    public MyModel UpdateModel(MyModel data)
    {
        var domainModel = Convert(data);
        var updatedDomainModel = _repository.UpdateModel(domainModel);
        return Convert(updatedDomainModel);
    }

    public bool DeleteModel(Guid id)
    {
        return _repository.DeleteModel(id);
    }

    private MyModel Convert(MyDomainModel model)
    {
        if (model == null) return null;
        return new MyModel(model.Data, model.Id);
    }

    private MyDomainModel Convert(MyModel model)
    {
        if (model == null) return null;
        return new MyDomainModel(model.Data, model.Id);
    }
}