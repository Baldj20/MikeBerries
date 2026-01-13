namespace UserService.BLL.Common;

public interface IRequestHandler<TIn, TOut>
{
    Task<TOut?> Handle(TIn request, CancellationToken cancellationToken);
}