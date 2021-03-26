using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollow : MonoBehaviour
{
    //Array de transforms utilizados como pontos, para os quais o personagem (objeto desse script) ir� se mover
    public Transform[] wayPoints;

    //Vari�vel de �ndice, destaca qual ponto est� sendo utilizado atualmente ("pontos" s�o os transforms acima)
    int wayPointIndex;

    //Velocidade na qual o personagem se move at� seu destino
    public float moveSpeed;

    //� usado para saber o qu�o pr�ximo o personagem precisa estar do seu destino para que possa prosseguir at� o pr�ximo ponto
    public float accuracy;

    //Velocidade de rota��o do personagem
    public float rotationSpeed;

    //LateUpdate ocorre ap�s os outros Updates (Update padr�o ou FixedUpdate)
    private void LateUpdate()
    {
        //Caso a Array de pontos estiver vazia, nada das linhas posteriores (exceto a linha abaixo) � executado
        if (wayPoints.Length == 0) return;


        //Posi��o final para qual o personagem deve se rotacionar, utilizar a posi��o Y do personagem faz com que sua rota��o X se mantenha a mesma na utiliza��o do vetor
        Vector3 lookAtGoal = new Vector3(wayPoints[wayPointIndex].position.x, transform.position.y, wayPoints[wayPointIndex].position.z);

        //Dira��o atual para qual o personagem vai se movimentar
        //Subtrair um Vetor por outro, gera um vetor que pode ser utilizado na dire��o de um vetor at� o outro
        Vector3 direction = lookAtGoal - transform.position;

        //Altera a rota��o do personagem para que ele se vire gradualmente at� seu destino
        //Os m�todos "Lerp" ou "Slerp" retornam a interpola��o entre um primeiro vetor e um segundo (nesse caso seria de um Quaternion)
        //O terceiro par�metro indica o qu�o alta � essa interpola��o, se for 0, o retorno ser� igual ao vetor do primeiro par�metro, se for 1, ser� igual ao segundo
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed); //(rotationSpeed / angleValue));

        //Caso a magnitude do vetor seja menor que a precis�o, o personagem parte para a pr�xima a��o
        //"Magnitude" se refere a qu�o longo � um vetor, nesse caso, a magnitude age como identifica��o da dist�ncia entre o personagem e seu destino
        //Se a magnitude est� muito pequena, significa que o personagem est� perto do destino
        if (direction.magnitude < accuracy)
        {
            //O valor do �ndice de pontos � aumentado em 1, significa que o ponto utilizado ser� o pr�ximo na array
            wayPointIndex++;

            //Caso o valor aumentado na linha anterior seja maior que a quantidade de pontos existentes na array, o �ndice se torna 0
            //Isso acontece pois caso o contr�rio ocorresse, o script estaria utilizando um ponto inexistente na array
            if (wayPointIndex >= wayPoints.Length)
            {
                //�ndice se torna 0 de acordo com as condi��es descritas nos coment�rios acima
                wayPointIndex = 0;
            }
        }

        //O personagem adiciona movimento em rela��o ao seu pr�prio eixo Z, significa que ele estar� se movimentando para a frente de s� mesmo
        //Esse m�todo funciona para que o personagem se mova at� os pontos, pois devido � rota��o, a frente do personagem estar� sempre se direcionando at� seu ponto de destino
        //O valor "Time.deltaTime" � utilizado no c�lculo pois com ele, os m�todos podem acontecer corretamente indepentendemente do desempenho do jogo em m�quinas diferentes
        //Se "Time.deltaTime" n�o fosse utilizado, haveria a impress�o de que o personagem estaria se movendo mais devagar, caso o jogo estivesse sendo executado em com um desempenho muito baixo
        transform.Translate(0, 0, moveSpeed * Time.deltaTime);

    }
}
