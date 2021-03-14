using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSObstacleTransparent : MonoBehaviour
{
    public struct St_ObstacleRendererInfo
    {
        public int InstanceId;
        public MeshRenderer Mesh_Renderer;
        public Shader OrinShader;
        public Color reColor;
    }

    public Transform playerTransform;
    public Transform cameraTransform;
    public Transform cameraArmTransform;

    private Dictionary<int, St_ObstacleRendererInfo> Dic_SavedObstaclesRendererInfo = new Dictionary<int, St_ObstacleRendererInfo>();
    private List<St_ObstacleRendererInfo> Lst_TransparentedRenderer = new List<St_ObstacleRendererInfo>();
    private Color ColorTransparent = new Color(1f, 1f, 1f, 0.2f);
    private Color ColorOrin = new Color(1f, 1f, 1f, 1f);
    private string ShaderColorParamName = "_Color";
    private Shader TransparentShader;
    private RaycastHit[] TransparentHits;
    // private LayerMask TransparentRayLayer;

    void Start()
    {
        // TransparentRayLayer = 1 << LayerMask.NameToLayer(DefineKey.LayerObstacle);
        TransparentShader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
    }

    void Update()
    {
        Camera_TransparentProcess_Operation();
    }

    void Camera_TransparentProcess_Operation()
    {
        if (playerTransform == null) return;


        // 반투명했던거 다시 원래 쉐이더로 복귀
        if (Lst_TransparentedRenderer.Count > 0)
        {
            for (int i = 0; i < Lst_TransparentedRenderer.Count; i++)
            {
                Lst_TransparentedRenderer[i].Mesh_Renderer.material.shader = Lst_TransparentedRenderer[i].OrinShader;
                Lst_TransparentedRenderer[i].Mesh_Renderer.material.color = Lst_TransparentedRenderer[i].reColor;
            }

            Lst_TransparentedRenderer.Clear();
        }


        // Vector3 playPos = playerTransform.position + playerTransform.TransformDirection(0, 1.5f, 0);
        Vector3 playPos = cameraArmTransform.position;
        float distance = (cameraTransform.position - playPos).magnitude; // 카메라와 플레이어 사이의 간격

        Vector3 dirToCam = (cameraTransform.position - playPos).normalized;  // 플레이어기준 카메라가 위치한 방향 
        // Vector3 DirToPlaybehind = -playerTransform.forward;   // 뒤
        

        //// 플레이어 뒤 오브젝트 반투명
        //HitRayTransparentObject(playPos, DirToPlaybehind, Distance);
        // 카메라암에서 카메라사이 오브젝트 반투명
        HitRayTransparentObject(playPos, dirToCam, distance);
    }


    void HitRayTransparentObject(Vector3 start, Vector3 direction, float distance)
    {
        TransparentHits = Physics.RaycastAll(start, direction, distance /*, TransparentRayLayer*/);  // 레이가 관통하여 계속 검사함

        Debug.Log(TransparentHits.Length + "개의 오브젝트 반투명 작동");
        // 레이와 충돌된 오브젝트가 있다면
        for (int i = 0; i < TransparentHits.Length; i++)    
        {
            // 레이와 충돌된 오브젝트의 고유 ID
            int instanceid = TransparentHits[i].collider.GetInstanceID();

            // 레이에 총돌한 장애물이 컬렉션에 없으면 저장하기
            // ContainsKey() : 키가 중복 된다면 true
            if (!Dic_SavedObstaclesRendererInfo.ContainsKey(instanceid))    
            {
                // 레이에 충돌된 오브젝트의 메쉬렌더러를 가져옴
                MeshRenderer obsRenderer = TransparentHits[i].collider.gameObject.GetComponent<MeshRenderer>(); 
                St_ObstacleRendererInfo rendererInfo = new St_ObstacleRendererInfo();

                // 고유 인스턴스아이디
                rendererInfo.InstanceId = instanceid;
                // 메시렌더러
                rendererInfo.Mesh_Renderer = obsRenderer;
                // 장애물의쉐이더 쉐이더 복구용 저장
                rendererInfo.OrinShader = obsRenderer.material.shader;
                rendererInfo.reColor = obsRenderer.material.color;

                // [] 사이에 Key를 입력하고 해당 Value를 대입
                Dic_SavedObstaclesRendererInfo[instanceid] = rendererInfo;  
            }

            // 쉐이더 반투명으로 변경
            Dic_SavedObstaclesRendererInfo[instanceid].Mesh_Renderer.material.shader = TransparentShader;
            // 알파값 줄인 쉐이더 색 변경
            Dic_SavedObstaclesRendererInfo[instanceid].Mesh_Renderer.material.SetColor(ShaderColorParamName, ColorTransparent);
            // 투명화 시킨 쉐이더
            Lst_TransparentedRenderer.Add(Dic_SavedObstaclesRendererInfo[instanceid]);
        }
    }
}
