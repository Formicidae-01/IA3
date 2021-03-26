using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollow : MonoBehaviour
{
    //Array de transforms utilizados como pontos, para os quais o personagem (objeto desse script) irá se mover
    public Transform[] wayPoints;

    //Variável de índice, destaca qual ponto está sendo utilizado atualmente ("pontos" são os transforms acima)
    int wayPointIndex;

    //Velocidade na qual o personagem se move até seu destino
    public float moveSpeed;

    //É usado para saber o quão próximo o personagem precisa estar do seu destino para que possa prosseguir até o próximo ponto
    public float accuracy;

    //Velocidade de rotação do personagem
    public float rotationSpeed;

    //LateUpdate ocorre após os outros Updates (Update padrão ou FixedUpdate)
    private void LateUpdate()
    {
        //Caso a Array de pontos estiver vazia, nada das linhas posteriores (exceto a linha abaixo) é executado
        if (wayPoints.Length == 0) return;


        //Posição final para qual o personagem deve se rotacionar, utilizar a posição Y do personagem faz com que sua rotação X se mantenha a mesma na utilização do vetor
        Vector3 lookAtGoal = new Vector3(wayPoints[wayPointIndex].position.x, transform.position.y, wayPoints[wayPointIndex].position.z);

        //Diração atual para qual o personagem vai se movimentar
        //Subtrair um Vetor por outro, gera um vetor que pode ser utilizado na direção de um vetor até o outro
        Vector3 direction = lookAtGoal - transform.position;

        //Altera a rotação do personagem para que ele se vire gradualmente até seu destino
        //Os métodos "Lerp" ou "Slerp" retornam a interpolação entre um primeiro vetor e um segundo (nesse caso seria de um Quaternion)
        //O terceiro parâmetro indica o quão alta é essa interpolação, se for 0, o retorno será igual ao vetor do primeiro parâmetro, se for 1, será igual ao segundo
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed); //(rotationSpeed / angleValue));

        //Caso a magnitude do vetor seja menor que a precisão, o personagem parte para a próxima ação
        //"Magnitude" se refere a quão longo é um vetor, nesse caso, a magnitude age como identificação da distância entre o personagem e seu destino
        //Se a magnitude está muito pequena, significa que o personagem está perto do destino
        if (direction.magnitude < accuracy)
        {
            //O valor do índice de pontos é aumentado em 1, significa que o ponto utilizado será o próximo na array
            wayPointIndex++;

            //Caso o valor aumentado na linha anterior seja maior que a quantidade de pontos existentes na array, o índice se torna 0
            //Isso acontece pois caso o contrário ocorresse, o script estaria utilizando um ponto inexistente na array
            if (wayPointIndex >= wayPoints.Length)
            {
                //índice se torna 0 de acordo com as condições descritas nos comentários acima
                wayPointIndex = 0;
            }
        }

        //O personagem adiciona movimento em relação ao seu próprio eixo Z, significa que ele estará se movimentando para a frente de sí mesmo
        //Esse método funciona para que o personagem se mova até os pontos, pois devido à rotação, a frente do personagem estará sempre se direcionando até seu ponto de destino
        //O valor "Time.deltaTime" é utilizado no cálculo pois com ele, os métodos podem acontecer corretamente indepentendemente do desempenho do jogo em máquinas diferentes
        //Se "Time.deltaTime" não fosse utilizado, haveria a impressão de que o personagem estaria se movendo mais devagar, caso o jogo estivesse sendo executado em com um desempenho muito baixo
        transform.Translate(0, 0, moveSpeed * Time.deltaTime);

    }
}
