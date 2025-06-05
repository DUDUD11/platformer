using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static FlatPhysics.FlatBody;

namespace FlatPhysics
{


    public sealed class FlatWorld
    {

        private readonly int world_layer = 4;

        public static readonly float MinBodySize = 0.01f * 0.01f;
        public static readonly float MaxBodySize = 1000f * 1000f;

        public static readonly float MinDensity = 0.5f; // g/cm^3;
        public static readonly float MaxDensity = 21.4f;

        public static readonly int MinIterations = 1;
        public static readonly int MaxIterations = 128;

        public List<FlatBody>[] bodyList;



        private FlatVector gravity;
        private List<(FlatBody,FlatBody)> contactTuple;
        public HashSet<(FlatBody, FlatBody)> CollideTuple;
        public HashSet<(FlatBody, FlatBody)> CollideTile;

        private FlatVector[] contactList;
        private FlatVector[] impulseList;
        private FlatVector[] frictionImplseList;
        private FlatVector[] raList;
        private FlatVector[] rbList;
        private float[] jList;
        

        //   public List<FlatVector> contactPointList;


        public enum Wolrd_layer
        {
            Hero_allias = 0,
            Mob_allias = 1,
            Static_allias = 2,
            None_Interact = 3
        }


        public FlatWorld()
        {
            // gravity = new FlatVector(0f, - 98.1f);
            gravity = new FlatVector(0f, -200f);
            bodyList = new List<FlatBody>[world_layer];
            for (int i = 0; i < world_layer; i++)
            {
                bodyList[i] = new List<FlatBody>();
            }
            this.contactTuple = new List<(FlatBody,FlatBody)>();
            this.CollideTuple = new HashSet<(FlatBody, FlatBody)>();
            this.CollideTile = new HashSet<(FlatBody, FlatBody)>();
            //   contactPointList = new List<FlatVector>();


            contactList = new FlatVector[2];
           impulseList = new FlatVector[2];
           frictionImplseList = new FlatVector[2];
           raList = new FlatVector[2];
           rbList = new FlatVector[2];
            jList = new float[2];
        }

        public void Clear()
        {
            for (int i = 0; i < world_layer; i++)
            {
                bodyList[i].Clear();
            }
        }

        public void AddBody(FlatBody body,Wolrd_layer wolrd_Type)
        {
            bodyList[(int)wolrd_Type].Add(body);
        }

        public bool RemoveBody(FlatBody body, Wolrd_layer wolrd_Type)
        {
            bool remove = bodyList[(int)wolrd_Type].Remove(body);


            return remove;
        }

        public bool GetBody(int index, out FlatBody body, Wolrd_layer wolrd_Type) 
        {

            body = null;

            if (index < 0 || index >= bodyList[(int)wolrd_Type].Count)
            {
                return false;
            }
            body = bodyList[(int)wolrd_Type][index];

            return true; 
        }

        public void StepWithoutGravity(float time, int iterations)
        {
            iterations = FlatMath.Clamp(iterations, MinIterations, MaxIterations);
            CollideTuple.Clear();
            CollideTile.Clear();
            //  this.contactPointList.Clear();

            for (int it = 0; it < iterations; it++)
            {

                contactTuple.Clear();
                StepBodiesWithoutGraivty(time, iterations);
                BroadPhase();
                NarrowPhase();

            }
        }


        public void Step(float time, int iterations)
        {
            iterations = FlatMath.Clamp(iterations, MinIterations, MaxIterations);
            CollideTuple.Clear();
            CollideTile.Clear();
            //  this.contactPointList.Clear();

            for (int it = 0; it < iterations; it++)
            {

                contactTuple.Clear(); 
                StepBodies(time, iterations);
                BroadPhase();
                NarrowPhase();

            }



        }

        private void SeparateBodies(FlatBody bodyI, FlatBody bodyJ, FlatVector mtv)
        {
     
            if (bodyI.isStatic)
            {
                bodyJ.Move(mtv);
            }

            else if (bodyJ.isStatic)
            {
                bodyI.Move(-mtv);
            }

            else
            {
                bodyI.Move(-mtv / 2f);
                bodyJ.Move(mtv / 2f);
            }



        }

        private void BroadPhase()
        {


            for (int i = 0; i < bodyList[0].Count; i++)
            {
                FlatBody bodyI = bodyList[0][i];
                FlatAABB bodyI_aabb = bodyI.GetAABB();

                for (int j = 0; j < bodyList[1].Count; j++)
                {
                    FlatBody bodyJ = bodyList[1][j];
                    FlatAABB bodyJ_aabb = bodyJ.GetAABB();



                    if (!bodyI.active || !bodyJ.active)
                    {
                        continue;
                    }

                    if (bodyI.isStatic && bodyJ.isStatic)
                    {
                        continue;
                    }

                    if (!Collisions.IntersectAABBs(bodyI_aabb, bodyJ_aabb))
                    {
                        continue;
                    }

                 

                    contactTuple.Add((bodyI, bodyJ));

                }


            }

            for (int i = 0; i < bodyList[1].Count; i++)
            {
                FlatBody bodyI = bodyList[1][i];
                FlatAABB bodyI_aabb = bodyI.GetAABB();

                if (bodyI.OnlyHero) continue;


                for (int j = 0; j < bodyList[2].Count; j++)
                {
                    FlatBody bodyJ = bodyList[2][j];
                    FlatAABB bodyJ_aabb = bodyJ.GetAABB();

                    if (!bodyI.active || !bodyJ.active)
                    {
                        continue;
                    }

                    if (bodyI.isStatic && bodyJ.isStatic)
                    {
                        continue;
                    }

                    if (!Collisions.IntersectAABBs(bodyI_aabb, bodyJ_aabb))
                    {
                        continue;
                    }

                    contactTuple.Add((bodyI, bodyJ));
                }


            }

            for (int i = 0; i < bodyList[0].Count; i++)
            {
                FlatBody bodyI = bodyList[0][i];
                FlatAABB bodyI_aabb = bodyI.GetAABB();

                for (int j = 0; j < bodyList[2].Count; j++)
                {
                    FlatBody bodyJ = bodyList[2][j];
                    FlatAABB bodyJ_aabb = bodyJ.GetAABB();

                    if (!bodyI.active || !bodyJ.active)
                    {
                        continue;
                    }

                    if (bodyI.isStatic && bodyJ.isStatic)
                    {
                        continue;
                    }

                    if (!Collisions.IntersectAABBs(bodyI_aabb, bodyJ_aabb))
                    {
                        continue;
                    }
               
                    contactTuple.Add((bodyI, bodyJ));

                }


            }

        }


       


        private void NarrowPhase()
        {
            for (int i = 0; i < contactTuple.Count; i++)
            {
                (FlatBody,FlatBody ) quad = contactTuple[i];
                FlatBody bodyI = quad.Item1;
                FlatBody bodyJ = quad.Item2;

            


                if (bodyI.isTile && bodyJ.isTile)
                {
                    continue;
                }

                    if (Collisions.Collide(bodyI, bodyJ, out FlatVector normal, out float depth))
                    {

                    if (bodyI.isTile || bodyJ.isTile)
                    {
           
                        CollideTile.Add((bodyI,bodyJ));
                        //   SeparateBodies(bodyI, bodyJ, normal * depth);

                        //   continue;
                    }

                    else
                    {
                        CollideTuple.Add((bodyI, bodyJ));
                    }

                    if (bodyI.OnlyHero || bodyJ.OnlyHero)
                    {
                        continue;
                        // OnlyHero의 경우 충돌처리는 무시하도록한다. 일단
                    }

                    SeparateBodies(bodyI, bodyJ, normal * depth);
                    Collisions.FindContactPoints(bodyI, bodyJ, out FlatVector contact1, out FlatVector contact2, out int contactCount);
                    FlatManifold contact = new FlatManifold(bodyI, bodyJ, normal, depth, contact1, contact2, contactCount);
                    ResolveCollisionWithRotationAndFriction(in contact);

                }

                /*
                debug
                if (it == iterations - 1)
                {

                    //if (!contactPointList.Contains(contact.Contact1))
                    //{ 
                    //    this.contactPointList.Add(contact.Contact1);
                    //}

                    //if (contact.ContactCount > 1)
                    //{
                    //    if (!contactPointList.Contains(contact.Contact2))
                    //    {
                    //        this.contactPointList.Add(contact.Contact2);
                    //    } 
                    //}
                }

                */

            }
        }

        public void StepBodies(float time, int totaliterations)
        {
            for (int iter = 0; iter < world_layer; iter++)
            {
                for (int i = 0; i < bodyList[iter].Count; i++)
                {
                    bodyList[iter][i].Step(time, gravity, totaliterations);

                }
            }

            //Hero
            bodyList[0][0].Angle = 0f;
           
        }

        public void StepBodiesWithoutGraivty(float time, int totaliterations)
        {
            for (int iter = 0; iter < world_layer; iter++)
            {

                for (int i = 0; i < bodyList[iter].Count; i++)
                {
                    bodyList[iter][i].Step(time, FlatVector.Zero, totaliterations);

                }
            }
            //Hero
            bodyList[0][0].Angle = 0f;
        }

        public void ResolveCollisionBasic(in FlatManifold contact)
        {
            FlatBody bodyA = contact.bodyA;
            FlatBody bodyB = contact.bodyB;
            FlatVector normal = contact.Normal;
            float depth = contact.depth; 

            FlatVector relativeVelocity = bodyB.LinearVelocity - bodyA.LinearVelocity;

            if (FlatMath.Dot(relativeVelocity, normal) > 0f)
            {
                return;
            }


            float e= MathF.Min(bodyA.restitution, bodyB.restitution);
            float j = -(1f + e) * FlatMath.Dot(relativeVelocity,normal);
            j/= bodyA.invMass + bodyB.invMass;

            FlatVector impulse = j * normal;

            bodyA.LinearVelocity -= impulse *bodyA.invMass;
            bodyB.LinearVelocity += impulse * bodyB.invMass;
        
        }

        public void ResolveCollisionWithRotation(in FlatManifold contact)
        {
            FlatBody bodyA = contact.bodyA;
            FlatBody bodyB = contact.bodyB;
            FlatVector normal = contact.Normal;
            FlatVector contact1 = contact.Contact1;
            FlatVector contact2 = contact.Contact2;
            int contactCount = contact.ContactCount;

            float e = MathF.Min(bodyA.restitution, bodyB.restitution);

            contactList[0] = contact1;
            contactList[1] = contact2;

            for (int i = 0; i < contactCount; i++)
            {
                impulseList[i] = FlatVector.Zero;
                raList[i] = FlatVector.Zero;
                rbList[i] = FlatVector.Zero;
            }

            for (int i = 0; i < contactCount; i++)
            {
                FlatVector ra = contactList[i] - bodyA.Position;
                FlatVector rb = contactList[i] - bodyB.Position;

                raList[i] = ra;
                rbList[i] = rb;

                FlatVector raPerp = new FlatVector(-ra.Y, ra.X);
                FlatVector rbPerp = new FlatVector(-rb.Y, rb.X);

                FlatVector angularLinearVelocityA = raPerp * bodyA.AngularVelocity;
                FlatVector angularLinearVelocityB = rbPerp * bodyB.AngularVelocity;

                FlatVector relativeVelocity = (bodyB.LinearVelocity+ angularLinearVelocityB)
                    - (bodyA.LinearVelocity + angularLinearVelocityA);

                float ContactVelocityMag = FlatMath.Dot(relativeVelocity, normal);

                if (ContactVelocityMag > 0f)
                {
                    continue;
                }

                float raPerpDotN = FlatMath.Dot(raPerp, normal);
                float rbPerpDotN = FlatMath.Dot(rbPerp, normal);

                float denom = bodyA.invMass + bodyB.invMass +
                    (raPerpDotN*raPerpDotN*bodyA.invInertia)+ (rbPerpDotN * rbPerpDotN * bodyB.invInertia);

                float j = -(1f + e) * ContactVelocityMag;
                j /= denom;
                j /= (float)contactCount;

                FlatVector impulse = j * normal;
                impulseList[i] = impulse;
            }

            for (int i = 0; i < contactCount; i++)
            {
                FlatVector impulse = impulseList[i];
           
                bodyA.LinearVelocity += -impulse * bodyA.invMass;
                bodyA.AngularVelocity += -FlatMath.Cross(raList[i], impulse ) * bodyA.invInertia;
                bodyB.LinearVelocity += impulse * bodyB.invMass;
                bodyB.AngularVelocity += FlatMath.Cross(rbList[i], impulse) * bodyB.invInertia;

          

            }



        }

        public void ResolveCollisionWithRotationAndFriction(in FlatManifold contact)
        {
            FlatBody bodyA = contact.bodyA;
            FlatBody bodyB = contact.bodyB;
            FlatVector normal = contact.Normal;
            FlatVector contact1 = contact.Contact1;
            FlatVector contact2 = contact.Contact2;
            int contactCount = contact.ContactCount;

            float e = MathF.Min(bodyA.restitution, bodyB.restitution);

            float sf = (bodyA.StaticFriction + bodyB.StaticFriction) * 0.5f;
            float df = (bodyA.DynamicFriction + bodyB.DynamicFriction) * 0.5f;

            this.contactList[0] = contact1;
            this.contactList[1] = contact2;

            for (int i = 0; i < contactCount; i++)
            {
                this.impulseList[i] = FlatVector.Zero;
                this.raList[i] = FlatVector.Zero;
                this.rbList[i] = FlatVector.Zero;
                this.frictionImplseList[i] = FlatVector.Zero;
                this.jList[i] = 0f;
            }

            for (int i = 0; i < contactCount; i++)
            {
                FlatVector ra = contactList[i] - bodyA.Position;
                FlatVector rb = contactList[i] - bodyB.Position;

                raList[i] = ra;
                rbList[i] = rb;

                FlatVector raPerp = new FlatVector(-ra.Y, ra.X);
                FlatVector rbPerp = new FlatVector(-rb.Y, rb.X);

                FlatVector angularLinearVelocityA = raPerp * bodyA.AngularVelocity;
                FlatVector angularLinearVelocityB = rbPerp * bodyB.AngularVelocity;

                FlatVector relativeVelocity =
                    (bodyB.LinearVelocity + angularLinearVelocityB) -
                    (bodyA.LinearVelocity + angularLinearVelocityA);

                float contactVelocityMag = FlatMath.Dot(relativeVelocity, normal);

                if (contactVelocityMag > 0f )
                {
                    continue;
                }

                float raPerpDotN = FlatMath.Dot(raPerp, normal);
                float rbPerpDotN = FlatMath.Dot(rbPerp, normal);

                float denom = bodyA.invMass + bodyB.invMass +
                    (raPerpDotN * raPerpDotN) * bodyA.invInertia +
                    (rbPerpDotN * rbPerpDotN) * bodyB.invInertia;

                float j = -(1f + e) * contactVelocityMag;
                j /= denom;
                j /= (float)contactCount;

                jList[i] = j;

                FlatVector impulse = j * normal;
                impulseList[i] = impulse;
            }

            for (int i = 0; i < contactCount; i++)
            {
                FlatVector impulse = impulseList[i];
                FlatVector ra = raList[i];
                FlatVector rb = rbList[i];

                bodyA.LinearVelocity += -impulse * bodyA.invMass;
                bodyA.AngularVelocity += -FlatMath.Cross(ra, impulse) * bodyA.invInertia;
                bodyB.LinearVelocity += impulse * bodyB.invMass;
                bodyB.AngularVelocity += FlatMath.Cross(rb, impulse) * bodyB.invInertia;
            }



            for (int i = 0; i < contactCount; i++)
            {
                FlatVector ra = contactList[i] - bodyA.Position;
                FlatVector rb = contactList[i] - bodyB.Position;

                raList[i] = ra;
                rbList[i] = rb;

                FlatVector raPerp = new FlatVector(-ra.Y, ra.X);
                FlatVector rbPerp = new FlatVector(-rb.Y, rb.X);

                FlatVector angularLinearVelocityA = raPerp * bodyA.AngularVelocity;
                FlatVector angularLinearVelocityB = rbPerp * bodyB.AngularVelocity;

                FlatVector relativeVelocity =
                    (bodyB.LinearVelocity + angularLinearVelocityB) -
                    (bodyA.LinearVelocity + angularLinearVelocityA);

                FlatVector tangent = relativeVelocity - FlatMath.Dot(relativeVelocity, normal) * normal;

                if (FlatMath.AbsoultelyEqual(tangent, FlatVector.Zero))
                {
                    continue;
                }
                else
                {
                    tangent = FlatMath.Normalize(tangent);
                }

                float raPerpDotT = FlatMath.Dot(raPerp, tangent);
                float rbPerpDotT = FlatMath.Dot(rbPerp, tangent);

                float denom = bodyA.invMass + bodyB.invMass +
                    (raPerpDotT * raPerpDotT) * bodyA.invInertia +
                    (rbPerpDotT * rbPerpDotT) * bodyB.invInertia;

                float jt = -FlatMath.Dot(relativeVelocity, tangent);
                jt /= denom;
                jt /= (float)contactCount;

                FlatVector frictionImpulse;
                float j = jList[i];

                if (MathF.Abs(jt) <= j * sf)
                {
                    frictionImpulse = jt * tangent;
                }
                else
                {
                    frictionImpulse = -j * tangent * df;
                }

                this.frictionImplseList[i] = frictionImpulse;
            }

            for (int i = 0; i < contactCount; i++)
            {
                FlatVector frictionImpulse = this.frictionImplseList[i];
                FlatVector ra = raList[i];
                FlatVector rb = rbList[i];

                bodyA.LinearVelocity += -frictionImpulse * bodyA.invMass;
                bodyA.AngularVelocity += -FlatMath.Cross(ra, frictionImpulse) * bodyA.invInertia;
                bodyB.LinearVelocity += frictionImpulse * bodyB.invMass;
                bodyB.AngularVelocity += FlatMath.Cross(rb, frictionImpulse) * bodyB.invInertia;
            }
        }


    }
}
