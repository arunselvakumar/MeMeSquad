﻿namespace Jericho.Identity.Models
{
    using System;

    public abstract class MongoUserContactRecord : IEquatable<MongoUserEmail>
    {
        protected MongoUserContactRecord(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.Value = value;
        }


        public string Value { get; }

        public ConfirmationOccurrence ConfirmationRecord { get; private set; }


        public bool Equals(MongoUserEmail other)
        {
            return other.Value.Equals(Value);
        }


        public bool IsConfirmed()
        {
            return ConfirmationRecord != null;
        }


        public void SetConfirmed()
        {
            SetConfirmed(new ConfirmationOccurrence());
        }

        public void SetConfirmed(ConfirmationOccurrence confirmationRecord)
        {
            if (ConfirmationRecord == null)
            {
                ConfirmationRecord = confirmationRecord;
            }
        }

        public void SetUnconfirmed()
        {
            ConfirmationRecord = null;
        }
    }
}