import {writable} from 'svelte/store'

const store = writable({
    fechaFutura: new Date("Jan 1 2021 00:00:00"),
    fechaActual: new Date()
})

export const cuentaRegresivaStore = store