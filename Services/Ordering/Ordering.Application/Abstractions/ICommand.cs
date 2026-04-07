namespace Ordering.Application.Abstractions;

//marker interface not returning anything
public interface ICommand
{
}

//marker interface returning result
public interface ICommand<TResult>
{

}