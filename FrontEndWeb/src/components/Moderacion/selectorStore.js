
import {writable} from 'svelte/store'

let  selectorStore = writable({
    activado: true,
    seleccionados: new Set()
})
selectorStore.selecionar =  function selecionar (id) {
    selectorStore.update(v => {
        if(v.seleccionados.has(id)) {
            v.seleccionados.delete(id)
        } else {
            v.seleccionados.add(id)
        }
        v.seleccionados = v.seleccionados
        return v
    })
}
export default selectorStore
