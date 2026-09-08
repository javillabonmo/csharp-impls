#https://docs.python.org/3/glossary.html#term-generator

# la palabra yield se utiliza para volver un generador la funcion
# la funcions es ejecutada hasta que se encuentra la palabra yield, en ese momento se devuelve el valor y se pausa la ejecucion de la funcion
# la proxima vez que se llame a la funcion, la ejecucion se reanuda desde el punto donde se habia pausado, y continua hasta encontrar otro yield o hasta que la funcion termine o retorne


def create_message_generator():
    yield "hi"
    yield "there"
    yield "friend"


gen = create_message_generator()
first = next(gen) # se necesita llamar a next para obtener el primer valor del generador y guardalo en una variable, para continuar en la misma instancia mas que crear una nueva
print(first)  # prints: hi
second = next(gen)
print(second)  # prints: there
third = next(gen)
print(third)  # prints: friend