using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GOAPPlanner : MonoBehaviour
{
    /**
     * Este método realiza un plan de acciones para cumplir un objetivo 'goal'
     * Devuelve una lista vacía de acciones si no se ha encontrado un plan, o una lista de acciones
     * a ejecutar, en orden, para cumplir el objetivo.
     */
    public List<GOAPAction> GetPlan(List<GOAPState> currentState, List<GOAPState> goal) {
        // obtener las acciones que puede hacer el agente
        List<GOAPAction> usableActions = FindObjectsOfType<GOAPAction>().ToList();
        List<Node> leaves = new List<Node>();

        // construir grafo
        Node start = new Node(null, 0, currentState, null);
        bool success = buildGraph(start, leaves, usableActions, goal);
        // si tenemos una solución, procesar
        if (success) {
            // obtener nodo más barato
            Node cheapest = null;
            foreach (Node leaf in leaves) {
                if (cheapest == null)
                    cheapest = leaf;
                else {
                    if (leaf.runningCost < cheapest.runningCost)
                        cheapest = leaf;
                }
            }
            // procesar todos sus padres comenzando desde el nodo final
            List<GOAPAction> result = new List<GOAPAction>();
            Node n = cheapest;
            while (n != null) {
                if (n.action != null) {
                    result.Insert(0, n.action); // insertar accion al principio
                }
                n = n.parent;
            }
            return result;
        } else {
            return new List<GOAPAction>();
        }
    }

    /**
	 * Devuelve true si al menos se ha encontrado una solución
	 * Los caminos posibles se almacenan en leaves. Cada leaf tiene un valor en
	 * 'runningCost' que se puede usar para determinar el plan más barato
	 */
    private bool buildGraph(Node parent, List<Node> leaves, List<GOAPAction> usableActions, List<GOAPState> goal) {
        bool foundOne = false;

        // miramos cada accion y vemos si podemos usarla
        foreach (GOAPAction action in usableActions) {

            // si el estado padre contiene las mismas condiciones que necesita la accion, la podemos usar
            if (inState(action.Preconditions, parent.state)) {

                // aplicar los efectos de la acción, de manera temporal, al estado padre
                List<GOAPState> currentState = populateState(parent.state, action.Effects);
                
                Node node = new Node(parent, parent.runningCost + action.cost, currentState, action);

                if (inState(goal, currentState)) {
                    // hemos encontrado una solución cuando coinciden los estados del goal con el estado actual
                    leaves.Add(node);
                    foundOne = true;
                } else {
                    // si no es una solución todavía, comprobamos el resto de acciones y creamos nueva rama en el grafo
                    List<GOAPAction> subset = actionSubset(usableActions, action);
                    bool found = buildGraph(node, leaves, subset, goal);
                    if (found)
                        foundOne = true;
                }
            }
        }

        return foundOne;
    }

    /**
     * Obtenemos un subconjunto de acciones, eliminando la accion removeMe
     */
    private List<GOAPAction> actionSubset(List<GOAPAction> actions, GOAPAction removeMe) {
        List<GOAPAction> subset = new List<GOAPAction>();
        foreach (GOAPAction a in actions) {
            if (!a.Equals(removeMe))
                subset.Add(a);
        }
        return subset;
    }

    /**
	 * Comprobarmos que todos los estados dentro de 'test' concuerdan con 'state'
	 * Si no se cumplen todos devolvemos false.
	 */
    private bool inState(List<GOAPState> test, List<GOAPState> state) {
        bool allMatch = true;
        foreach (GOAPState t in test) {
            bool match = false;
            foreach (GOAPState s in state) {
                if (s.key == t.key && s.val == t.val) {
                    match = true;
                    break;
                }
            }
            if (!match)
                allMatch = false;
        }
        return allMatch;
    }

    /**
	 * Aplicar el cambio de estado al estado actual
	 */
    private List<GOAPState> populateState(List<GOAPState> currentState, List<GOAPState> stateChange) {
        List<GOAPState> state = new List<GOAPState>();
        foreach (GOAPState s in currentState) {
            state.Add(new GOAPState(s.key, s.val));
        }

        foreach (GOAPState change in stateChange) {
            bool exists = false;

            foreach (GOAPState s in state) {
                if (s.key == change.key) {
                    exists = true;
                    break;
                }
            }
            // si existe el estado, actualizamos el valor
            if (exists) {
                state.RemoveAt(state.FindIndex(x => x.key == change.key));
                GOAPState updated = new GOAPState(change.key, change.val);
                state.Add(updated);
            }
            // si no existe el estado, lo añadimos
            else {
                state.Add(new GOAPState(change.key, change.val));
            }
        }
        return state;
    }
    /* 
     * Node representa cada uno de los nodos del grafo
     */
    private class Node {
        public Node parent;
        public float runningCost;
        public List<GOAPState> state;
        public GOAPAction action;

        public Node(Node parent, float runningCost, List<GOAPState> state, GOAPAction action) {
            this.parent = parent;
            this.runningCost = runningCost;
            this.state = state;
            this.action = action;
        }
    }

}