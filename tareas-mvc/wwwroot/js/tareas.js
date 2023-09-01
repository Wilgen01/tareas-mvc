function agregarNuevaTareaAlListado() {
    tareasLostadoViewModel.tareas.push(new tareaElementoListadoViewModel({ id: 0, titulo: "" }))
    $("[name=titulo-tarea]").last().focus()
}

async function manejarFocusoutTituloTarea(tarea) {
    const titulo = tarea.titulo();
    if (!titulo) {
        tareasLostadoViewModel.tarea.pop();
        return
    }

    const data = JSON.stringify(titulo);
    const respuesta = await fetch(urlTareas, {
        method: 'POST',
        body: data,
        headers: { "Content-Type": "application/json" }
    });

    if (respuesta.ok) {
        const json = await respuesta.json();
        tarea.id(json.id);
    } else {
        console.log("No se pudo obtener la data");
    }

}

async function obtenerTareas() {
    tareasLostadoViewModel.cargando(true)

    const respuesta = await fetch(urlTareas, {
        method: 'GET',
        headers: { "Content-Type": "application/json" }
    })

    if (!respuesta.ok) {
        return
    }

    const json = await respuesta.json();
    tareasLostadoViewModel.tareas([]);

    json.forEach(valor => {
        tareasLostadoViewModel.tareas.push(new tareaElementoListadoViewModel(valor))
    })

    tareasLostadoViewModel.cargando(false)

}


async function manejarClickTarea(tarea) {
    if (tarea.esNuevo()) {
        return;
    }

    const respuesta = await fetch(`${urlTareas}/${tarea.id()}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (!respuesta.ok) {
        alert('Hubo un error')
        return
    }


    const json = await respuesta.json();
    editarTareaVM.id = json.id;
    editarTareaVM.titulo(json.titulo);
    editarTareaVM.descripcion(json.descripcion);

    modalEditarTareaBootstrap.show();

}