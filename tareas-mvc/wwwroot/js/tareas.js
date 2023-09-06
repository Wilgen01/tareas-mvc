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

    editarTareaVM.pasos([])

    json.pasos.forEach(paso => {
        editarTareaVM.pasos.push(
            new pasoViewModel({...paso, modoEdicion: false})
        )
    })

    modalEditarTareaBootstrap.show();

}

async function manejarCambioEditarTarea() {
    if (!editarTareaVM.titulo().trim()) return 
    const tarea = {
        id: editarTareaVM.id,
        titulo: editarTareaVM.titulo().trim(),
        descripcion: editarTareaVM.descripcion()
    }

    await editarTarea(tarea);

    const indice = tareasLostadoViewModel.tareas().findIndex(t => t.id() == tarea.id)
    tareasLostadoViewModel.tareas()[indice].titulo(tarea.titulo)
}

async function editarTarea(tarea) {
    const json = JSON.stringify(tarea);

    const respuesta = await fetch(`${urlTareas}/${tarea.id}`, {
        method: 'PUT',
        body: json,
        headers: {
            'Content-Type': 'application/json'
        }
    })

    if (!respuesta.ok) {
        alert('Hubo un error')
    }
}

async function intentarBorrarTarea(tarea) {
    const userConfirm = confirm(`Está seguro de que desea eliminar la tarea "${editarTareaVM.titulo().trim()}"`)

    if (!userConfirm) return

    await borrarTarea(tarea.id);

    const indice = tareasLostadoViewModel.tareas().findIndex(t => t.id() == tarea.id)
    tareasLostadoViewModel.tareas.splice(indice, 1)
    modalEditarTareaBootstrap.hide();
}

async function borrarTarea(id) {
    const respuesta = await fetch(`${urlTareas}/${id}`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        }
    })

    if (!respuesta.ok) {
        alert('Hubo un error')
    }
}