using FluentValidation;

namespace KampusRitim.Application.UseCases.Event.GetAllEvent
{
    public class GetAllEventValidation : AbstractValidator<GetAllEventRequest>
    {
        public GetAllEventValidation()
        {
            // Parametre olmadığı için kural yok.
        }
    }
}

