import App from './App.svelte';
import Navbar from './components/Navbar.svelte';
import HiloList from './components/Hilos/HiloList.svelte';
import Administracion from './components/Administracion/Administracion.svelte';
import Moderacion from './components/Paginas/Moderacion.svelte';
import Login from './components/Paginas/Login.svelte';
import Registro from './components/Paginas/Registro.svelte';
import Inicio from './components/Paginas/Inicio.svelte';
import Token from './components/Paginas/Token.svelte';
import HistorialUsuario from './components/Paginas/HistorialUsuario.svelte';
import Domado from './components/Paginas/Domado.svelte';
import ListaDeUsuarios from './components/Paginas/ListaDeUsuarios.svelte';
import Busqueda from './components/Paginas/Busqueda.svelte';
import EliminadosYDesactivados from './components/Paginas/EliminadosYDesactivados.svelte';
import MediaHome from './components/Paginas/MediaHome.svelte';
import Historial from './components/Paginas/Historial.svelte';

import skinStore from './components/Personalizacion/skinsStore'

skinStore.applicarEstilo()

// const app = new App({
// 	target: document.body,
// 	props: {
// 		name: 'world'
// 	}
// });

let componentes = [
	["#svelte", App, {}],
	["#svelte-navbar", Navbar, {}],
	["#svelte-index", HiloList, {hiloList: window.hiloList}],
	["#svelte-administracion", Administracion, {}],
	["#svelte-moderacion", Moderacion, {}],
	["#svelte-token", Token, {}],
	["#svelte-login", Login, {}],
	["#svelte-historialDeUsuario", HistorialUsuario, {}],
	["#svelte-registro", Registro],
	["#svelte-inicio", Inicio],
	["#svelte-domado", Domado, {}],
	["#svelte-listaDeUsuarios", ListaDeUsuarios, {}],
	["#svelte-eliminadosYDesactivados", EliminadosYDesactivados, {}],
	["#svelte-media-home", MediaHome, {}],
	["#svelte-busqueda", Busqueda, {}],
	["#svelte-historial", Historial, {}],
]

for (const c of componentes) {
	if(document.querySelector(c[0]))
	{	
		new c[1]({
			target: document.querySelector(c[0]),
			props: c[2]
		});
	}
	
}