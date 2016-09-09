// ---------------------------------------------------------------------------
// <copyright file="Zoo.cs" company="Boathill">
//   Copyright (c) Jason Zhu.  All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------

namespace Coding.App
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Library;

    /// <summary>
    /// CareerCup Interview Question (http://www.careercup.com/question?id=20043669)
    /// Write a simple Zoo simulator which contains 3 different types of animal: 
    /// monkey, giraffe and elephant. The zoo should open with 5 of each type of animal.
    /// Each animal has a health value held as a percentage (100% is completely healthy). 
    /// Every animal starts at 100% health. This value should be a floating point value.
    /// The application should act as a simulator, with time passing at the rate of 1 hour with each interation. 
    /// Every hour that passes, a random value between 0 and 20 is to be generated for each animal. 
    /// This value should be passed to the appropriate animal, whose health is then reduced by that percentage of their current health.
    /// The user must be able to feed the animals in the zoo. 
    /// When this happens, the zoo should generate three random values between 10 and 25; one for each type of animal. 
    /// The health of the respective animals is to be increased by the specified percentage of their current health. Health should be capped at 100%.
    /// When an Elephant has a health below 70% it cannot walk. 
    /// If its health does not return above 70% once the subsequent hour has elapsed, it is pronounced dead.
    /// When a Monkey has a health below 30%, or a Giraffe below 50%, it is pronounced dead straight away. 
    /// The user interface should show the current status of each Animal and contain two buttons, 
    /// one to provoke an hour of time to pass and another to feed the zoo. 
    /// The UI should update to reflect each change in state, and the current time at the zoo.
    /// </summary>

    public class Animal
    {
        protected static float MIN_LIVES_VALUE = 0.0f;
        protected static float MAX_HEATH_VALUE = 100.0f;

        #region Fields

        protected float _healthValue = MAX_HEATH_VALUE;
        protected float _previousAHV = MAX_HEATH_VALUE;

        #endregion

        #region Constructors

        #endregion

        #region Properties

        public virtual bool CanWalk
        {
            get { return this._healthValue > MIN_LIVES_VALUE; }
        }

        public float HealthValue
        {
            get { return this._healthValue; }
            protected set
            {
                this._previousAHV = this._healthValue;

                if (value < 0.0)
                {
                    this._healthValue = 0.0f;
                }
                else if (value > MAX_HEATH_VALUE)
                {
                    this._healthValue = MAX_HEATH_VALUE;
                }
                else // value in range
                {
                    this._healthValue = value;
                }
            }
        }

        public virtual bool IsDead
        {
            get { return !this.IsLive; }
        }

        public virtual bool IsLive
        {
            get { return this._healthValue > MIN_LIVES_VALUE; }
        }

        #endregion

        #region Functions

        #endregion

        #region Methods

        public void Feed(float feedValue)
        {
            this.HealthValue += feedValue;
        }

        public void Live(float costValue)
        {
            this.HealthValue -= costValue;
        }

        #endregion

    }

    public class Elephant : Animal
    {
        protected new static float MIN_LIVES_VALUE = 70.0f;

        public override bool CanWalk
        {
            get
            {
                return this._healthValue < MIN_LIVES_VALUE;
            }
        }

        public override bool IsLive
        {
            get
            {
                return this._healthValue < MIN_LIVES_VALUE && this._previousAHV < MIN_LIVES_VALUE;
            }
        }
    }

    public class Giraffe : Animal
    {
        protected new static float MIN_LIVES_VALUE = 50.0f;

        public override bool IsLive
        {
            get { return this._healthValue >= MIN_LIVES_VALUE; }
        }

    }

    public class Monkey : Animal
    {
        protected new static float MIN_LIVES_VALUE = 30.0f;

        public override bool IsLive
        {
            get { return this._healthValue >= MIN_LIVES_VALUE; }
        }

    }

    public class Zoo
    {
        protected static float MIN_FEED_VALUE = 10.0f;
        protected static float MAX_FEED_VALUE = 25.0f;
        protected static float MIN_PERCENTAGE = 0.0f;
        protected static float MAX_PERCENTAGE = 0.2f;

        #region Fields

        private List<Animal> _animals = new List<Animal>();

        #endregion

        #region Constructors

        #endregion

        #region Properties

        public List<Animal> Animals
        {
            get { return _animals; }
        }

        #endregion

        #region Functions

        #endregion

        #region Methods

        public void Feed(Animal animal)
        {
            float feedValue = (float) RandomHelper.NextDouble(MIN_FEED_VALUE, MAX_FEED_VALUE);

            animal.Feed(feedValue);
        }

        public void FeedAll()
        {
            foreach(var animal in _animals)
            {
                this.Feed(animal);
            }
        }

        public void Live()
        {
            foreach(var animal in _animals)
            {
                float currentHeathValue = animal.HealthValue;
                float percentage = (float) RandomHelper.NextDouble(0, 0.2);
                float costValue = currentHeathValue * percentage;

                animal.Live(costValue);
            }
        }

        #endregion

    }

}
