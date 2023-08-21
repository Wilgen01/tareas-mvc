function agregarNuevaTareaAlListado() {
    tareasLostadoViewModel.tareas.push(new tareaElementoListadoViewModel({ id: 0, titulo: "" }))
    $("[name=titulo-tarea]").last().focus()
}

function manejarFocusoutTituloTarea(tarea) {
    const titulo = tarea.titulo();
    if (!titulo) {
        tareasLostadoViewModel.tarea.pop();
        return
    }

    tarea.id(1);
}