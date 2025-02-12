using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews;

public static class ReviewErrors
{
    public static readonly Error NotElegible = new Error("Review.NotElegible", "Este review y calificacion para el auto no es elegible porque aun no se completa el alquiler");
}