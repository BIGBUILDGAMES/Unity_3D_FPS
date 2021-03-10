using System.Collections;
using UnityEngine;
using UnityEngine.AI; // AI, 내비게이션 시스템 관련 코드를 가져오기

// 적 AI를 구현한다
public class Enemy : LivingEntity {
    public LayerMask whatIsTarget; // 추적 대상 레이어
    private Collider plCollider; // 플레이어 콜라이더
    private Transform plTransform; // 플레이어 트랜스폼

    private LivingEntity nexusEntity; // 추적할 대상
    private LivingEntity playerEntity; // 추적할 대상
    private NavMeshAgent pathFinder; // 경로계산 AI 에이전트

    public ParticleSystem hitEffect; // 피격시 재생할 파티클 효과
    public ParticleSystem boomEffect; // 피격시 재생할 파티클 효과
    public AudioClip deathSound; // 사망시 재생할 소리
    public AudioClip hitSound; // 피격시 재생할 소리

    private Animator enemyAnimator; // 애니메이터 컴포넌트
    private AudioSource enemyAudioPlayer; // 오디오 소스 컴포넌트
    private Renderer enemyRenderer; // 렌더러 컴포넌트

    private float Distance = 20f;
    private float revengeTime = 0f;

    public float damage; // 공격력
    public float timeBetAttack = 5f; // 공격 간격

    private bool isCatch;
    private bool isAttack;
    private bool findPlayer;
    private bool revenge;
    private int enemyKind;

    private Plane[] frustum;
    private Bounds plBounds;    // playerBounds
    public Camera eye;
    private bool isVisible;
    private GameObject player;
    private GameObject nexus;

    private LineRenderer bulletLineRenderer; // 탄알 궤적을 그리기 위한 렌더러

    // 추적할 대상이 존재하는지 알려주는 프로퍼티
    //private bool hasTarget
    //{
    //    get
    //    {
    //        // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
    //        if (targetEntity != null && !targetEntity.dead)
    //        {
    //            return true;
    //        }

    //        // 그렇지 않다면 false
    //        return false;
    //    }
    //}

    private void Awake()
    {
        // 게임 오브젝트로부터 사용할 컴포넌트 가져오기
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        enemyAudioPlayer = GetComponent<AudioSource>();

        // 렌더러 컴포넌트는 자식 게임 오브젝트에 있으므로
        // GetComponentInChildren() 메서드 사용
        enemyRenderer = GetComponentInChildren<Renderer>();

        bulletLineRenderer = GetComponent<LineRenderer>();
        // 사용할 점을 두 개로 변경
        bulletLineRenderer.positionCount = 2;
        // 라인 렌더러를 비활성화
        bulletLineRenderer.enabled = false;

        nexus = GameObject.Find("Nexus");
        nexusEntity = nexus.GetComponent<LivingEntity>();

        player = GameObject.Find("Player");
        plCollider = player.GetComponent<Collider>();
        plTransform = player.GetComponent<Transform>();
        playerEntity = player.GetComponent<LivingEntity>();
    }

    // 적 AI의 초기 스펙을 결정하는 셋업 메서드
    public void Setup(float newHealth, float newDamage, float newSpeed, int kind)
    {
        // 체력 설정
        startingHealth = health = newHealth;
        // 공격력 설정
        damage = newDamage;
        // 내비메시 에이전트의 이동 속도 설정
        pathFinder.speed = newSpeed;
        // 렌더러가 사용 중인 머티리얼의 컬러를 변경, 외형 색이 변함
        // enemyRenderer.material.color = skinColor;
        enemyKind = kind;
    }

    private void Start() {
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        //StartCoroutine(UpdatePath());
        isCatch = false;
        isAttack = false;
    }

    private void Update()
    {
        if (!dead)
        {
            if (!isCatch)
            // 추적 대상의 존재 여부에 따라 다른 애니메이션을 재생
            {
                enemyAnimator.SetBool("HasTarget", true);
            }
            else
            {
                enemyAnimator.SetBool("HasTarget", false);
            }

            // 시야 절두체
            frustum = GeometryUtility.CalculateFrustumPlanes(eye);
            plBounds = plCollider.bounds;
            isVisible = GeometryUtility.TestPlanesAABB(frustum, plBounds);
            if (isVisible)
            {
                FindPlayer();
            }
            else
            {
                findPlayer = false;
            }

            if (findPlayer || revenge)
                pathFinder.SetDestination(player.transform.position);
            else
                pathFinder.SetDestination(nexus.transform.position);

            if (revenge)
            {
                revengeTime += Time.deltaTime;
                if (revengeTime >= 3.0f)
                {
                    revenge = false;
                }
            }
            else
            {
                revengeTime = 0.0f;
            }
        }
    }

    // 주기적으로 추적할 대상의 위치를 찾아 경로를 갱신
    //private IEnumerator UpdatePath()
    //{
    //    // 살아있는 동안 무한 루프
    //    while (!dead)
    //    {
    //        if (findPlayer)
    //        {
    //            if (hasTarget)
    //            {
    //                // 추적 대상 존재 : 경로를 갱신하고 AI 이동을 계속 진행
    //                pathFinder.isStopped = false;
    //                pathFinder.SetDestination(targetEntity.transform.position);
    //            }
    //            else
    //            {
    //                // 추적 대상 없음: AI 이동 중지
    //                pathFinder.isStopped = true;

    //                // 20유닛의 반지름을 가진 가상의 구를 그렸을 때 구와 겹치는 모든 콜라이더를 가져옴
    //                // 단, whatIsTarget 레이어를 가진 콜라이더만 가져오도록 필터링
    //                Collider[] colliders =
    //                    Physics.OverlapSphere(transform.position, 200f, whatIsTarget);

    //                // 모든 콜라이더를 순회하면서 살아 있는 LivingEntity 찾기
    //                for (int i = 0; i < colliders.Length; i++)
    //                {
    //                    // 콜라이더로부터 LivingEntity 컴포넌트 가져오기
    //                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
    //                    // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아 있다면
    //                    if (livingEntity != null && !livingEntity.dead)
    //                    {
    //                        // 추적 대상을 해당 LivingEntity로 설정
    //                        targetEntity = livingEntity;

    //                        // for 문 루프 즉시 정지
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //        // 0.25초 주기로 처리 반복
    //        yield return new WaitForSeconds(0.25f);
    //    }
    //}

    // 데미지를 입었을때 실행할 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal) 
    {
        // 아직 사망하지 않은 경우에만 피격 효과 재생
        if (!dead)
        {
            // 공격받은 지점과 방향으로 파티클 효과 재생
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            //    // 피격 효과음 재생
            //    enemyAudioPlayer.PlayOneShot(hitSound);        

            // LivingEntity의 OnDamage()를 실행하여 데미지 적용
            base.OnDamage(damage, hitPoint, hitNormal);
            revenge = true;
            Debug.Log("hit");
        }
    }

    // 사망 처리
    public override void Die() 
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();

        // 다른 AI를 방해하지 않도록 자신의 모든 콜라이더를 비활성화
        Collider[] enemyColliders = GetComponents<CapsuleCollider>();
        Rigidbody enemyRigidbody = GetComponent<Rigidbody>();
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        boomEffect.Play();

        // AI 추적을 중지하고 내비메시 컴포넌트 비활성화
        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        // 사망 애니메이션 재생
        enemyAnimator.SetTrigger("Die");
        // 사망 효과음 재생
        //enemyAudioPlayer.PlayOneShot(deathSound);
    }

    private void OnTriggerStay(Collider other) 
    {
        // 자신이 사망하지 않았으며
        if (!dead)
        {
            if ((!findPlayer && !revenge) || findPlayer)
            {
                // 상대방의 LivingEntity 타입 가져오기 시도
                LivingEntity attackTarget = other.GetComponent<LivingEntity>();

                // 상대방의 LivingEntity가 자신의 추적 대상이라면 공격 실행
                if (attackTarget != null &&
                    (attackTarget == nexusEntity || attackTarget == playerEntity))
                {
                    isCatch = true;
                    enemyAnimator.SetTrigger("Attack");
                    pathFinder.isStopped = true;

                    // 상대방의 피격 위치와 피격 방향을 근삿값으로 계산
                    Vector3 hitPoint = other.ClosestPoint(transform.position);
                    Vector3 hitNormal = transform.position - other.transform.position;

                    if (isAttack)
                    {
                        // 공격 실행
                        attackTarget.OnDamage(damage, hitPoint, hitNormal);
                        isAttack = false;
                    }
                }
            }
            else
            {
                pathFinder.isStopped = false;
                Debug.Log("sfdfsdfdff");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 자신이 사망하지 않았으며
        if (!dead)
        {
            // 상대방의 LivingEntity 타입 가져오기 시도
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            if (attackTarget != null &&
                    (attackTarget == nexusEntity || attackTarget == playerEntity))
            {
                isCatch = false;
                pathFinder.isStopped = false;
            }
        }
    }

    public void Attack()
    {
        isAttack = true;
    }

    private void FindPlayer()
    {
        // 레이캐스트에 의한 충돌 정보를 저장하는 컨테이너
        RaycastHit hit;
        // 탄알이 맞은 곳을 저장할 변수
        Vector3 hitPosition = Vector3.zero;
        Vector3 direction = (plTransform.position - transform.position).normalized;


        // 레이캐스트(시작 지점, 방향, 충돌 정보 컨테이너, 사정거리)
        if (Physics.Raycast(eye.transform.position, direction, out hit, Distance))
        {
            // 레이가 어떤 물체와 충돌한 경우

            // 충돌한 상대방으로부터 콜라이더 가져오기 시도
            Collider target = hit.collider.GetComponent<Collider>();

            // 플레이어를 감지 했다면
            if (target != null && target.tag == "Player")
            {
                findPlayer = true;
                revenge = false;
                Debug.Log(hit.transform.gameObject.name);
            }
            else if (target == null || (target.tag != "Player" && target.tag != "Enemy"))
            {
                findPlayer = false;
            }

            if (findPlayer || revenge)
            {
                // 선의 시작점은 총구의 위치
                bulletLineRenderer.SetPosition(0, eye.transform.position);

                // 선의 끝점은 입력으로 들어온 충돌 위치
                bulletLineRenderer.SetPosition(1, plTransform.position);

                // 라인 렌더러를 활성화하여 탄알 궤적을 그림
                bulletLineRenderer.enabled = true;              
            }
        }

        //    // 레이가 충돌한 위치 저장
        //    hitPosition = hit.point;
        //}
        //else
        //{
        //    // 레이가 다른 물체와 충돌하지 않았다면
        //    // 탄알이 최대 사정거리까지 날아갔을 때의 위치를 충돌 위치로 사용
        //    hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        //}
    }
}

