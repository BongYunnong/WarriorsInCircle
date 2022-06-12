using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb2D;
    [SerializeField] float speed = 1f;
    float damage = 1f;
    [SerializeField] float lifeTime = 0.5f;
    private bool destroying = false;
    [SerializeField] int teamIndex = 0;

    [SerializeField] Sprite[] bulletSprites;
    [SerializeField] ParticleSystem[] yellParticles;

    [SerializeField] AudioSource bulletAudioSource;
    [SerializeField] AudioClip[] bulletAudioclips;

    [SerializeField] AudioSource hitAudioSource;
    [SerializeField] AudioClip[] characterHitAudioClips;
    [SerializeField] AudioClip[] wallHitAudioClips;
    [SerializeField] AudioClip[] bulletHitAudioClips;

    [SerializeField] GameObject[] bulletHitVFXs;

    public void InitializeBullet(float _damage, Vector2 _input, bool _leftAttack,int _teamIndex, Color _color_l, Color _color_r)
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.velocity = _input*speed;

        damage = _damage;

        Vector3 dir = _input;
        dir.z = 0f;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg-90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        GetComponent<SpriteRenderer>().color = _leftAttack ? _color_l : _color_r;

        teamIndex = _teamIndex;
        if (teamIndex == 0)
        {
            GetComponent<SpriteRenderer>().sprite = bulletSprites[GameData.weaponIndex];
        }

        bulletAudioSource.clip = bulletAudioclips[Random.Range(0, bulletAudioclips.Length)];
        bulletAudioSource.Play();

        yellParticles[teamIndex].Play();

        destroying = false;
        StartCoroutine("DestroyCoroutine", lifeTime);
    }

    IEnumerator DestroyCoroutine(float _lifeTime)
    {
        yield return new WaitForSeconds(_lifeTime);
        if (destroying == false)
        {
            destroying = true;
            rb2D.velocity = Vector2.zero;
            Destroy(rb2D);

            float destroyTime = 0.5f;
            while (destroyTime > 0)
            {
                float deltaT = Time.deltaTime;
                destroyTime -= deltaT;
                yield return new WaitForSeconds(deltaT);
                Color tmpColor = GetComponent<SpriteRenderer>().color;
                tmpColor.a = destroyTime / 0.5f;
                GetComponent<SpriteRenderer>().color = tmpColor;
            }

            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destroying==false)
        {
            GameManager _gm = GameManager.GetInstance();
            if (collision.transform.CompareTag("Player") && teamIndex == 1)
            {
                hitAudioSource.clip = characterHitAudioClips[Random.Range(0, characterHitAudioClips.Length)];
                hitAudioSource.Play();
                StartCoroutine("DestroyCoroutine", 0);
                collision.GetComponent<Character>().Attacked(
                    damage,
                    (collision.transform.position - this.transform.position).normalized
                    );
                _gm.CameraShake(damage, 0.1f);

                Instantiate(bulletHitVFXs[0], this.transform.position,Quaternion.identity);
            }
            else if (collision.transform.CompareTag("Enemy") && teamIndex == 0)
            {
                hitAudioSource.clip = characterHitAudioClips[Random.Range(0, characterHitAudioClips.Length)];
                hitAudioSource.Play();
                StartCoroutine("DestroyCoroutine", 0);
                collision.GetComponent<Character>().Attacked(
                    damage,
                    (collision.transform.position - this.transform.position).normalized);
                _gm.CameraShake(damage, 0.1f);

                Instantiate(bulletHitVFXs[0], this.transform.position, Quaternion.identity);
            }
            else if (collision.transform.CompareTag("Wall"))
            {
                hitAudioSource.clip = wallHitAudioClips[Random.Range(0, wallHitAudioClips.Length)];
                hitAudioSource.Play();
                StartCoroutine("DestroyCoroutine", 0);
                _gm.CameraShake(damage, 0.1f);

                Instantiate(bulletHitVFXs[1], this.transform.position, Quaternion.identity);
            }
            else if (collision.transform.CompareTag("Bullet"))
            {
                Bullet collBullet = collision.GetComponent<Bullet>();
                if (collBullet.teamIndex != teamIndex && collBullet.destroying == false)
                {
                    hitAudioSource.clip = bulletHitAudioClips[Random.Range(0, bulletHitAudioClips.Length)];
                    hitAudioSource.Play();
                    StartCoroutine("DestroyCoroutine", 0);
                    _gm.CameraShake(damage, 0.1f);

                    Instantiate(bulletHitVFXs[1], this.transform.position, Quaternion.identity);
                }
            }
        }
    }
}
