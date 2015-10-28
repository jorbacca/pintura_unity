using UnityEngine;
using System.Collections;

public class InterfaceInformation {

	public string id_estudiante;
	public string fecha;
	public string codigo_interfaz;
	public string paso;
	public string marcador_error;
	public string tipo_modo;

	public InterfaceInformation(string student_code, string date, string interf_code, string step, string marker_error, string type_mode){
		this.Id_estudiante = student_code;
		this.Fecha = date;
		this.Codigo_interfaz = interf_code;
		this.Paso = step;
		this.Marcador_error = marker_error;
		this.Tipo_modo = type_mode;
	}

	public string Id_estudiante {
		get {
			return id_estudiante;
		}
		set {
			id_estudiante = value;
		}
	}

	public string Fecha {
		get {
			return fecha;
		}
		set {
			fecha = value;
		}
	}

	public string Codigo_interfaz {
		get {
			return codigo_interfaz;
		}
		set {
			codigo_interfaz = value;
		}
	}

	public string Paso {
		get {
			return paso;
		}
		set {
			paso = value;
		}
	}

	public string Marcador_error {
		get {
			return marcador_error;
		}
		set {
			marcador_error = value;
		}
	}

	public string Tipo_modo {
		get {
			return tipo_modo;
		}
		set {
			tipo_modo = value;
		}
	}
}
