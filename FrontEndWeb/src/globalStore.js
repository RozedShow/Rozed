import { writable } from 'svelte/store';
import config from './config'
import Cookie from 'js-cookie'

let data = Object.assign({
    mostrarLogin: false,
    mostrarRegistro: false,
    debug: true,
    fondo: 'url(/imagenes/rosed.png)',
    esCelular: false,
}, window.globalState)

// Categorias 
data.categoriasActivas = config.categorias.filter(c => !c.nsfw).map(c => c.id)

if(Cookie.getJSON('categoriasActivas')) {
    data.categoriasActivas = Cookie.getJSON('categoriasActivas')
    // Quitar este despues (es para activar la categoria programacion automaticamente)
    data.categoriasActivas = [...data.categoriasActivas, 38]

} else
    Cookie.set('categoriasActivas', data.categoriasActivas)

// Hide comentarios
let comentariosOcultosStorage = localStorage.getItem('comentariosOcultos')
if(!comentariosOcultosStorage) comentariosOcultosStorage = JSON.stringify(['test'])
data.comentariosOcultos = new Map(JSON.parse(comentariosOcultosStorage).map(e => [e, true]))

// Checkeo si es celular
data.esCelular = window.innerWidth < 600

const store= writable(data)
export default  {
    subscribe: store.subscribe,
    set(value) {
        localStorage.setItem('comentariosOcultos', JSON.stringify(Array.from(value.comentariosOcultos.keys())))
        Cookie.set('categoriasActivas', value.categoriasActivas, { expires: 696969 })
        store.set(value)
    },
    update (config){
        Cookie.set('categoriasActivas', config.categoriasActivas, { expires: 696969 })
        store.update(config)
    }
}